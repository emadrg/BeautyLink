import { Box, Rating } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { roleTypes } from "../../constants";
import useClientStylistReviewService from "../../services/ClientStylistReviewService";
import useStylistClientReviewService from "../../services/StylistClientReviewService";
import userSession from "../../utils/userSession";

const ClientReceivedAndLeftReviews = () => {
  const [leftReviews, setLeftReviews] = useState([]);
  const [receivedReviews, setReceivedReviews] = useState([]);

  const { getStylistReviewsLeftByClient } = useClientStylistReviewService();

  const { getStylistClientReviewsByClientId } = useStylistClientReviewService();

  const user = userSession.user();

  const navigate = useNavigate();

  const fetchReceivedReviews = async () => {
    const reviews = await getStylistClientReviewsByClientId(user.id);
    setReceivedReviews(reviews);
  };

  const fetchLeftReviews = async () => {
    const reviews = await getStylistReviewsLeftByClient(user.id);
    setLeftReviews(reviews);
  };

  useEffect(() => {
    if (user == null || user.roleId != roleTypes.clientId) {
      navigate("/login");
    } else {
      fetchReceivedReviews();
      fetchLeftReviews();
    }
  }, []);

  return (
    <div>
      {user && user.roleId == roleTypes.clientId && (
        <div>
          <h1>{user.firstName}'s reviews</h1>
          <div className="stylist-all-reviews">
            <div className="stylist-left-reviews">
              <h2>Left reviews</h2>
              {leftReviews && (
                <div>
                  {leftReviews.map((review) => (
                    <div key={review.id} className="single-review-review-page">
                      <div>Left for: {review.stylistName} </div>
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
              {leftReviews.length == 0 && <div>No reviews left yet</div>}
            </div>
            <div className="stylist-received-reviews">
              <h2>Received reviews</h2>
              {!!receivedReviews.length && (
                <div>
                  {receivedReviews.map((review) => (
                    <div key={review.id} className="single-review-review-page">
                      <div> Left by: {review.stylistName} </div>
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
              {receivedReviews.length == 0 && (
                <div>No received reviews yet</div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ClientReceivedAndLeftReviews;
