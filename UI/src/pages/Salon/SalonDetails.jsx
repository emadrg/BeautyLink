import { Box, Button, Modal } from "@mui/material";
import { useEffect, useState } from "react";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import { useParams } from "react-router-dom";
import "../../../src/styles/base/base.scss";
import "../../../src/styles/pages/clientHomePage.scss";
import "../../../src/styles/pages/salonDetails.scss";
import CustomLoader from "../../components/utility/CustomLoader";
import useClientSalonReviewService from "../../services/ClientSalonReviewService";
import useSalonService from "../../services/SalonService";
import { getFile } from "../../utils";
import userSession from "../../utils/userSession";
import DisplayOrUpdateReview from "../Review/DisplayOrUpdateReview";
import SalonAllReviews from "./SalonAllReviews";
import ServiceWithStylists from "./ServiceWithStylists";

const SalonDetails = () => {
  const [salonWithDetails, setSalonWithDetails] = useState([]);
  const [arrayServiceStylists, setArrayServiceStylists] = useState(undefined);
  const [loadMore, setLoadMore] = useState(false);
  const [servicesWithStylists, setServicesWithStylists] = useState(undefined);
  const [loading, setLoading] = useState(true);
  const [imageLoader, setImageLoader] = useState(true);
  const [reviews, setReviews] = useState([]);
  const [hasReviews, setHasReviews] = useState(true);
  const [pictures, setPictures] = useState(undefined);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [index, setIndex] = useState(0);
  const [salonPosition, setSalonPosition] = useState(undefined);
  const [hasClientVisitedSalonState, setHasClientVisitedSalonState] =
    useState(undefined);
  const firstThreeServicesWithStylists = [];

  const { id: salonId } = useParams();
  const user = userSession.user();

  const { getSalonWithDetailsById, hasClientVisitedSalon } = useSalonService();
  const { getSalonPictures } = useSalonService();
  const { getClientSalonReviewsBySalonId } = useClientSalonReviewService();
  const { createClientSalonReview, updateClientSalonReview } =
    useClientSalonReviewService();

  const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 500,
    height: 500,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
  };

  const hasClientVisitedSalonFunc = async () => {
    setHasClientVisitedSalonState(await hasClientVisitedSalon(salonId));
  };

  const checkNumber = (number) => {
    if (number > pictures.length - 1) {
      return 0;
    }
    if (number < 0) {
      return pictures.length - 1;
    }
    return number;
  };

  const nextPicture = () => {
    setIndex((index) => {
      let newIndex = index + 1;
      return checkNumber(newIndex);
    });
    setModalIsOpen(true);
  };
  const prevPicture = () => {
    setIndex((index) => {
      let newIndex = index - 1;
      return checkNumber(newIndex);
    });
    setModalIsOpen(true);
  };

  const handleModalState = () => {
    setModalIsOpen(!modalIsOpen);
  };

  const fetchPictures = async () => {
    try {
      const picturesFromApi = await getSalonPictures(salonId);
      if (picturesFromApi.length != 0) {
        setPictures(picturesFromApi);
      }
      setImageLoader(false);
    } catch (error) {
      console.log(error);
      setImageLoader(false);
    }
  };

  const fetchSalon = async () => {
    setLoading(true);
    try {
      const salon = await getSalonWithDetailsById(salonId);
      setSalonWithDetails(salon);
      setSalonPosition({
        lat: salon.latitude,
        lng: salon.longitude,
      });
      let servicesWithStylists = salon.serviceStylists.reduce((acc, curr) => {
        if (acc[curr.service]) {
          acc[curr.service].push(curr);
        } else {
          acc[curr.service] = [curr];
        }
        return acc;
      }, {});
      setServicesWithStylists(servicesWithStylists);

      let arrayServiceStylists = [];
      Object.entries(servicesWithStylists).forEach(([key, value]) => {
        arrayServiceStylists.push(value);
      });
      setArrayServiceStylists(arrayServiceStylists);

      setLoading(false);
    } catch (error) {
      setLoading(false);
    }
  };

  const fetchReviews = async () => {
    setLoading(true);
    try {
      const fetchedReviews = await getClientSalonReviewsBySalonId(
        salonId,
        null,
        null
      );
      setReviews(fetchedReviews);
      setLoading(false);
      if (fetchedReviews.length > 0) {
        setHasReviews(true);
      } else {
        setHasReviews(false);
      }
    } catch (e) {
      setLoading(false);
      console.log(e);
    }
  };

  useEffect(() => {
    fetchSalon();

    fetchPictures();

    fetchReviews();
    if (user) {
      hasClientVisitedSalonFunc();
    }
  }, []);

  if (loading) {
    return <CustomLoader />;
  }

  return (
    <div>
      <div className="salon-details">
        <div className="salon-and-pictures" style={{ width: 900 }}>
          <h1>{salonWithDetails.salonName}</h1>
          <div>
            {salonWithDetails.address}
            {salonWithDetails.city} {salonWithDetails.county}{" "}
          </div>

          <div>
            {pictures != undefined && (
              <div style={{ display: "flex" }} onClick={handleModalState}>
                <img
                  src={getFile(pictures[0])}
                  style={{
                    width: 400,
                    height: 400,
                    objectFit: "cover",
                    padding: 10,
                  }}
                ></img>

                <div style={{ display: "grid" }}>
                  <img
                    src={getFile(pictures[1])}
                    style={{
                      width: 200,
                      height: 200,
                      objectFit: "cover",
                      padding: 10,
                      display: "grid",
                    }}
                  ></img>
                  <img
                    src={getFile(pictures[2])}
                    style={{
                      width: 200,
                      height: 200,
                      objectFit: "cover",
                      padding: 10,
                      display: "grid",
                    }}
                  ></img>
                </div>
              </div>
            )}
          </div>
          {loadMore && (
            <div>
              <div className="service-list" style={{ display: "block" }}>
                {Object.entries(servicesWithStylists).map(
                  ([service, stylists]) => (
                    <ServiceWithStylists
                      key={service}
                      stylists={stylists}
                      service={service}
                    ></ServiceWithStylists>
                  )
                )}
              </div>
              <button
                style={{ background: "#e3c3d2" }}
                onClick={() => setLoadMore(false)}
              >
                Show Less
              </button>
            </div>
          )}

          {!loadMore && arrayServiceStylists && (
            <div>
              <div className="service-list" style={{ display: "block" }}>
                {arrayServiceStylists.slice(0, 3).map((arr) => (
                  <ServiceWithStylists
                    key={arr[0].serviceId}
                    stylists={arr.map((elem) => ({
                      stylist: elem.stylist,
                      stylistId: elem.stylistId,
                    }))}
                    service={arr[0].service}
                  ></ServiceWithStylists>
                ))}
              </div>
              <button
                style={{ background: "#e3c3d2" }}
                onClick={() => setLoadMore(true)}
              >
                Show More
              </button>
            </div>
          )}

          {!pictures && !imageLoader && <div>No pictures yet</div>}

          {modalIsOpen && !imageLoader && pictures && (
            <div>
              <Modal open={modalIsOpen} onClose={() => handleModalState()}>
                <Box sx={{ ...style }}>
                  <img
                    src={getFile(pictures[index])}
                    className="display-profile-picture"
                  />
                  <div className="button-container">
                    <Button className="prev-btn" onClick={prevPicture}>
                      <FaChevronLeft />
                    </Button>
                    <Button className="next-btn" onClick={nextPicture}>
                      <FaChevronRight />
                    </Button>
                  </div>
                </Box>
              </Modal>
            </div>
          )}
          <div style={{ marginTop: 20 }}>
            {user && hasClientVisitedSalonState && (
              <DisplayOrUpdateReview
                existingReviewObject={reviews.find(
                  (review) => review.clientId === user.id
                )}
                updateReviewMethod={updateClientSalonReview}
                addReviewMethod={createClientSalonReview}
                receiverType="salonId"
              />
            )}
            {user && !hasClientVisitedSalonState && (
              <div>
                You have to vistit the salon first in order to leave a review
              </div>
            )}
            {!user && <div>You have to login in order to leave a review</div>}
          </div>
        </div>

        <div>
          <div className="reviews">
            {hasReviews && !loading && (
              <div>
                <div className="reviews-contents">
                  <h2>Salon reviews:</h2>
                  {reviews.map((review) => {
                    return <SalonAllReviews key={review.id} {...review} />;
                  })}
                </div>
              </div>
            )}
            {!hasReviews && !loading && <div>No reviews yet</div>}
          </div>

          {salonPosition != undefined && (
            <div className="map-salon-details">
              <MapContainer
                center={{
                  lat: salonPosition.lat,
                  lng: salonPosition.lng,
                }}
                zoom={13}
                style={{ height: 400, width: 400 }}
              >
                <TileLayer
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                  url="https://www.google.cn/maps/vt?lyrs=m@189&gl=cn&x={x}&y={y}&z={z}"
                />
                <Marker
                  position={{
                    lat: salonPosition.lat,
                    lng: salonPosition.lng,
                  }}
                >
                
                </Marker>
              </MapContainer>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default SalonDetails;
