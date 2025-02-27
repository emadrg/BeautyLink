import { Box, Rating } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { roleTypes } from "../../constants";
import useClientStylistReviewService from "../../services/ClientStylistReviewService";
import useStylistClientReviewService from "../../services/StylistClientReviewService";
import useStylistService from "../../services/StylistService";
import userSession from "../../utils/userSession";

const StylistReviews = () => {
  const [leftReviews, setLeftReviews] = useState(undefined);
  const [receivedReviews, setReceivedReviews] = useState(undefined);

  const { getStylistIdByUserId } = useStylistService();

  const { getClientStylistReviewsByStylistId } =
    useClientStylistReviewService();
  const { getClientReviewsLeftByStylist } = useStylistClientReviewService();

  const user = userSession.user();
  const navigate = useNavigate();
  const fetchReceivedReviews = async () => {
    let stylistId = await getStylistIdByUserId(user.id);
    const reviews = await getClientStylistReviewsByStylistId(stylistId);
    setReceivedReviews(reviews);
  };

  const fetchLeftReviews = async () => {
    let stylistId = await getStylistIdByUserId(user.id);
    const reviews = await getClientReviewsLeftByStylist(stylistId);
    setLeftReviews(reviews);
  };

  useEffect(() => {
    if (user == null || user.roleId != roleTypes.stylistId) {
      navigate("/login");
    } else {
      fetchReceivedReviews();
      fetchLeftReviews();
    }
  }, []);

  return (
    <div>
      {user && user.roleId == roleTypes.stylistId && (
        <div>
          <h1>{user.firstName}'s reviews</h1>
          <div className="stylist-all-reviews">
            <div className="stylist-left-reviews">
              <h2>Left reviews</h2>
              {leftReviews && (
                <div>
                  {leftReviews.map((review) => (
                    <div key={review.id} className="single-review-review-page">
                      <div>Left for: {review.clientName} </div>
                      <div>
                        <Box
                          component="fieldset"
                          mb={3}
                          borderColor="transparent"
                        >
                          <Rating
                            name="read-only"
                            value={review.score}
                            readOnly
                          />
                        </Box>
                        {""}
                      </div>{" "}
                      <i>{review.text} </i>
                    </div>
                  ))}
                </div>
              )}
            </div>
            <div className="stylist-received-reviews">
              <h2>Received reviews</h2>
              {receivedReviews && (
                <div>
                  {receivedReviews.map((review) => (
                    <div key={review.id} className="single-review-review-page">
                      <div>Left by: {review.clientName} </div>
                      <div>
                        <Box
                          component="fieldset"
                          mb={3}
                          borderColor="transparent"
                        >
                          <Rating
                            name="read-only"
                            value={review.score}
                            readOnly
                          />
                        </Box>
                        {""}
                      </div>{" "}
                      <i>{review.text} </i>
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default StylistReviews;
