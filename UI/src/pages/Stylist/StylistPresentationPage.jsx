import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useClientStylistReviewService from "../../services/ClientStylistReviewService";
import useStylistService from "../../services/StylistService";
import userSession from "../../utils/userSession";
import DisplayOrUpdateReview from "../Review/DisplayOrUpdateReview";
import Stylist from "./Stylist";

const StylistPresentationPage = () => {
  const { id: stylistId } = useParams();
  const user = userSession.user();
  const navigate = useNavigate();
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [hasClientVisitedStylistState, setHasClientVisitedStylistState] =
    useState(undefined);

  const { getClientStylistReviewsByStylistId } =
    useClientStylistReviewService();
  const { createClientStylistReview, updateClientStylistReview } =
    useClientStylistReviewService();
  const { hasClientVisitedStylist } = useStylistService();

  const hasClientVisitedStylistFunc = () => {
    var response = hasClientVisitedStylist(stylistId);
    setHasClientVisitedStylistState(response);
  };

  const fetchReviews = async (stylistId) => {
    try {
      console.log(user);
      const fetchedReviews = await getClientStylistReviewsByStylistId(
        stylistId
      );
      setReviews(fetchedReviews);
      setLoading(false);
    } catch (e) {
      setLoading(false);
      console.log(e);
    }
  };

  useEffect(() => {
    fetchReviews(stylistId);
    hasClientVisitedStylistFunc();
  }, []);

  const handleOnClick = () => {
    navigate(`/appointment/${stylistId}`);
  };
  return (
    <div>
      <Stylist></Stylist>
      {user && (
        <button onClick={handleOnClick} className="confirm-button">
          Make an appointment
        </button>
      )}

      {!user && (
        <div>You have to be logged in order to make an appointment.</div>
      )}

      {user != null && (
        <div className="display-or-update-review">
          {hasClientVisitedStylistState && (
            <DisplayOrUpdateReview
              existingReviewObject={reviews.find(
                (review) => review.clientId === user.id
              )}
              updateReviewMethod={updateClientStylistReview}
              addReviewMethod={createClientStylistReview}
              receiverType="stylistId"
            />
          )}
          {!hasClientVisitedStylistState && (
            <div>
              You have to visit this stylist first in order to leave a review
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default StylistPresentationPage;
