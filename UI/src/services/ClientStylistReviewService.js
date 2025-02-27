import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useClientStylistReviewService = () => {
  const api = useApi();

  const getClientStylistReviewsByStylistId = async (stylistId) => {
    const reviews = await api.get(
      ApiPaths.GetClientStylistReviewsByStylistId(stylistId)
    );
    return reviews;
  };

  const createClientStylistReview = async ({ stylistId, text, score }) => {
    await api.post(ApiPaths.CreateClientStylistReview, {
      stylistId,
      text,
      score,
    });
  };

  const updateClientStylistReview = async ({ stylistId, text, score }) => {
    const review = await api.post(ApiPaths.UpdateClientStylistReview, {
      stylistId,
      text,
      score,
    });
    return review;
  };

  const getStylistReviewsLeftByClient = async (clientId) => {
    const reviews = await api.get(
      ApiPaths.GetStylistReviewsLeftByClient(clientId)
    );
    return reviews;
  };

  return {
    getClientStylistReviewsByStylistId,
    createClientStylistReview,
    updateClientStylistReview,
    getStylistReviewsLeftByClient,
  };
};

export default useClientStylistReviewService;
