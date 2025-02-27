import { Box, Button, Modal } from "@mui/material";
import { useEffect, useState } from "react";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import "../../../src/styles/base/base.scss";
import "../../../src/styles/pages/clientHomePage.scss";
import "../../../src/styles/pages/salonDetails.scss";
import useManagerService from "../../services/ManagerService";
import useSalonService from "../../services/SalonService";
import "../../styles/pages/manager.scss";
import "../../styles/pages/stylistDetails.scss";
import { getFile } from "../../utils";
import userSession from "../../utils/userSession";

const ManagerHomePage = () => {
  const user = userSession.user();
  const [stylists, setStylists] = useState(undefined);
  const [salonWithDetails, setSalonWithDetails] = useState(undefined);
  const [loading, setLoading] = useState(true);
  const [imageLoader, setImageLoader] = useState(true);
  const [pictures, setPictures] = useState(undefined);
  const [stylistsAreLoading, setStylistsAreLoading] = useState(true);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [index, setIndex] = useState(0);
  const [managerId, setManagerId] = useState(undefined);
  const { getStylistsByManagerId, getManagerIdByUserId } = useManagerService();
  const { getSalonIdByManagerId, getSalonWithDetailsById, getSalonPictures } =
    useSalonService();
  const navigate = useNavigate();

  const getManagerIdByStylistId = async () => {
    let managerId = await getManagerIdByUserId(user.id);
    setManagerId(managerId);
  };

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

  const fetchStylist = async () => {
    try {
      let managerId = await getManagerIdByUserId(user.id);
      let stylists = await getStylistsByManagerId(managerId);
      setStylists(stylists);
      setStylistsAreLoading(false);
    } catch (e) {
      setStylistsAreLoading(false);
    }
  };

  const fetchPictures = async () => {
    try {
      let managerId = await getManagerIdByUserId(user.id);
      const salonId = await getSalonIdByManagerId(managerId);

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
      let managerId = await getManagerIdByUserId(user.id);
      const salonId = await getSalonIdByManagerId(managerId);
      const salon = await getSalonWithDetailsById(salonId);
      setSalonWithDetails(salon);
      setLoading(false);
    } catch (error) {
      setLoading(false);
    }
  };

  useEffect(() => {
    getManagerIdByStylistId();
    fetchStylist();
    fetchSalon();
    fetchPictures();
  }, []);

  return (
    <div>
      <div className="salon-and-pictures">
        {salonWithDetails && (
          <>
            <h1>{salonWithDetails.salonName}</h1>
            <div>
              {salonWithDetails.address}
              {salonWithDetails.city} {salonWithDetails.county}
            </div>
          </>
        )}

        <div>
          {pictures != undefined && (
            <div style={{ display: "flex" }} onClick={handleModalState}>
              <div>
                <img
                  className="salon-picture-big"
                  src={getFile(pictures[0])}
                ></img>
              </div>
              <div style={{ display: "grid" }}>
                <img
                  src={getFile(pictures[1])}
                  className="salon-picture-small"
                ></img>
                <img
                  src={getFile(pictures[2])}
                  className="salon-picture-small"
                ></img>
              </div>
            </div>
          )}
        </div>

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
      </div>
      {salonWithDetails && (
        <>
          {" "}
          <h2>Stylists working in {salonWithDetails.salonName}</h2>
          <div className="stylists-for-manager">
            {!stylistsAreLoading && (
              <>
                {stylists.map((stylist) => (
                  <div key={stylist.id}>
                    <div
                      className="stylist-card-for-manager"
                      onClick={() =>
                        navigate(`/stylistForManager/${stylist.id}`)
                      }
                    >
                      <div className="stylist-card-for-manager-name">
                        {stylist.firstName}
                        {stylist.lastName}
                      </div>
                      <div className="stylist-card-for-manager-image">
                        <img
                          src={getFile(stylist.profilePicture)}
                          style={{
                            width: 50,
                            height: 50,
                            display: "cover",
                            borderRadius: "50%",
                          }}
                        />
                      </div>
                    </div>
                  </div>
                ))}
              </>
            )}
          </div>
        </>
      )}
    </div>
  );
};

export default ManagerHomePage;
