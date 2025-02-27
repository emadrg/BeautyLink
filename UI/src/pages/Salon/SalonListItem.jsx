import { Box, Rating } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import CustomLoader from "../../components/utility/CustomLoader";
import resources from "../../resources/resources";
import useClientSalonReviewService from "../../services/ClientSalonReviewService";
import useSalonService from "../../services/SalonService";
import ApiPaths from "../../statics/ApiPaths";
import { getFile } from "../../utils";

const Salon = ({ id, name, county, city, address }) => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  const [loading, setLoading] = useState(true);
  const [reviews, setReviews] = useState([]);
  const [hasReviews, setHasReviews] = useState(true);
  const [pictures, setPictures] = useState(undefined);
  const [imageLoader, setImageLoader] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 3;

  const { getClientSalonReviewsBySalonId } = useClientSalonReviewService();
  const { getSalonPictures } = useSalonService();

  const fetchReviews = async (skip, take) => {
    try {
      const fetchedReviews = await getClientSalonReviewsBySalonId(
        id,
        skip,
        take
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

  const fetchPicture = async () => {
    try {
      const picturesFromApi = await getSalonPictures(id, 0, 1);
      if (picturesFromApi.length != 0) {
        setPictures(picturesFromApi);
      }
      setImageLoader(false);
    } catch (error) {
      console.log(error);
      setImageLoader(false);
    }
  };

  useEffect(() => {
    fetchReviews(0, pageSize);
    fetchPicture();
  }, []);

  useEffect(() => {
    fetchReviews(0, currentPage * pageSize);
  }, [currentPage]);

  if (loading) {
    return <CustomLoader />;
  }

  return (
    <div
      className="salon-card"
      onClick={() => navigate(ApiPaths.GetSalonWithDetails(id))}
    >
      <div className="salon-card-content">
        <div className="salon-card-text">
          {" "}
          <div>
            <b>
              {" "}
              {t(resources.salonListItem.salonLabel)} {name}
            </b>
          </div>
          <div>
            {t(resources.salonListItem.addressLabel)} {address}, {city} {county}
          </div>
          {hasReviews && (
            <div>
              <div>
                Total number of reviews: {""}
                {reviews.length}
                {""}
              </div>
              <div>
                <Box component="fieldset" mb={3} borderColor="transparent">
                  <Rating
                    name="read-only"
                    value={
                      reviews.reduce(
                        (acc, currentValue) => acc + currentValue.score,
                        0
                      ) / reviews.length
                    }
                    readOnly
                  />
                </Box>
                {""}
              </div>{" "}
            </div>
          )}
          {!hasReviews && <div>No reviews yet</div>}
        </div>

        <div className="salon-card-media">
          <>
            {!imageLoader && pictures && (
              <img
                src={getFile(pictures[0])}
                style={{
                  objectFit: "cover",
                  width: 150,
                  height: 150,
                  padding: 5,
                  borderRadius: 15,
                }}
              ></img>
            )}
          </>
          <>{!pictures && !imageLoader && <div></div>}</>
        </div>
      </div>
    </div>
  );
};

export default Salon;
