import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import useClientService from "../../services/ClientService";
import useStylistClientReviewService from "../../services/StylistClientReviewService";
import useStylistService from "../../services/StylistService";
import { getFile } from "../../utils";
import userSession from "../../utils/userSession";
import DisplayOrUpdateReview from "../Review/DisplayOrUpdateReview";
import DisplayAllReviews from "../Stylist/StylistAllReviews";

const ClientDetailsForStylist = () => {
  const [client, setClient] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [stylistId, setStylistId] = useState(undefined);
  const [picture, setPicture] = useState(undefined);
  const [reviews, setReviews] = useState(undefined);
  const user = userSession.user();

  const [reviewButtonisPressed, setReviewButtonIsPressed] = useState(false);

  const { createStylistClientReview, updateStylistClientReview } =
    useStylistClientReviewService();
  const handleReviewButtonPress = () => {
    setReviewButtonIsPressed(!reviewButtonisPressed);
  };

  const { getClientDetailsById } = useClientService();
  const { getStylistClientReviewsByClientId } = useStylistClientReviewService();
  const { getStylistIdByUserId } = useStylistService();
  const { id: clientId } = useParams();

  const fetchClientDetails = async () => {
    try {
      const client = await getClientDetailsById(clientId);
      setClient(client);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchStylistId = async () => {
    let stylistId = await getStylistIdByUserId(user.id);
    setStylistId(stylistId);
  };

  const fetchReviews = async () => {
    const reviews = await getStylistClientReviewsByClientId(clientId);
    setReviews(reviews);
  };

  useEffect(() => {
    fetchClientDetails();
    fetchReviews();
    fetchStylistId();
  }, []);

  return (
    <div>
      <div className="stylist-details">
        <div className="stylist-picture">
          {!isLoading && (
            <div>
              <h1>{client.firstName}</h1>
              <img
                src={getFile(client.profilePicture)}
               className="display-profile-picture"
              ></img>
            </div>
          )}
        </div>
        <div className="reviews-and-info">
          <div className="stylist-info">
            <div>{client.lastName}</div>
            <div>{client.email}</div>
            <div>{client.phoneNumber}</div>
          </div>
          <div className="reviews">
            {reviews && (
              <div>
                {reviews.map((review) => {
                  return <DisplayAllReviews key={review.id} {...review} />;
                })}
              </div>
            )}
          </div>
        </div>
      </div>
      {reviews && (
        <div className="display-or-update-review">
          <DisplayOrUpdateReview
            existingReviewObject={reviews.find(
              (review) => review.stylistId === stylistId
            )}
            updateReviewMethod={updateStylistClientReview}
            addReviewMethod={createStylistClientReview}
            receiverType="clientId"
          />
        </div>
      )}
    </div>
  );
};

export default ClientDetailsForStylist;
