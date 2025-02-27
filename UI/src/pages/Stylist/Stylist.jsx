import { Box, Rating } from "@mui/material";
import "font-awesome/css/font-awesome.min.css";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import "../../../src/styles/pages/stylistDetails.scss";
import CustomLoader from "../../components/utility/CustomLoader";
import useClientStylistReviewService from "../../services/ClientStylistReviewService";
import useStylistService from "../../services/StylistService";
import { getFile } from "../../utils";
import userSession from "../../utils/userSession";
import DisplayAllReviews from "./StylistAllReviews";

const Stylist = () => {
  const [stylist, setStylist] = useState("");
  const [loading, setLoading] = useState(true);
  const [reviews, setReviews] = useState([]);
  const [hasReviews, setHasReviews] = useState(false);
  const [reviewsAreExpanded, setReviewsAreExpanded] = useState(false);
  const [profilePicture, setProfilePicture] = useState(undefined);
  const [pictureisLoading, setPictureIsLoading] = useState(true);
  const { getStylistById, getStylistProfilePicture } = useStylistService();
  const { getClientStylistReviewsByStylistId } =
    useClientStylistReviewService();

  const { id: stylistId } = useParams();

  const user = userSession.user();

  const { t } = useTranslation();

  const fetchStylist = async () => {
    try {
      const stylist = await getStylistById(stylistId);
      setStylist(stylist);
      setLoading(false);
      console.log(stylist);
      const picture = await getStylistProfilePicture(stylist.userId);
      setProfilePicture(picture);
      setPictureIsLoading(false);
    } catch (e) {
      setLoading(false);
      setPictureIsLoading(false);
      console.log(e);
    }
  };

  const fetchReviews = async (stylistId) => {
    try {
      const fetchedReviews = await getClientStylistReviewsByStylistId(
        stylistId
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
    fetchStylist();
    fetchReviews(stylistId);
  }, []);

  if (loading) {
    return <CustomLoader />;
  }

  return (
    <div>
      <div className="stylist-details">
        <div className="stylist-picture">
          <h1>
            {stylist.firstName} {stylist.lastName}
            {hasReviews && (
              <div>
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
            {!pictureisLoading && (
              <div>
                <img
                  src={getFile(profilePicture)}
                  className="display-profile-picture"
                ></img>
              </div>
            )}
          </h1>
        </div>
        <div className="reviews-and-info">
          <div className="stylist-info">
            <div> Email: {stylist.email}</div>
            <div> Socila media link: {stylist.socialMediaLink}</div>
            <div>{stylist.phoneNumber}</div>
          </div>

          <div className="reviews">
            {hasReviews && user && (
              <div className="reviews-contents">
                <h2>Reviews:</h2>
                {reviews
                  .filter((review) => review.clientId != user.id)
                  .map((review) => {
                    return <DisplayAllReviews key={review.id} {...review} />;
                  })}
              </div>
            )}
            {hasReviews && !user && (
              <div className="reviews-contents">
                <h2>Reviews:</h2>
                {reviews.map((review) => {
                  return <DisplayAllReviews key={review.id} {...review} />;
                })}
              </div>
            )}
            {!hasReviews && <div>No reviews yet</div>}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Stylist;
