import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useStylistClientReviewService = () => {
  const api = useApi();

  const createStylistClientReview = async ({ clientId, text, score }) => {
    await api.post(ApiPaths.CreateStylistClientReview, {
      clientId,
      text,
      score,
    });
  };

  const getStylistClientReviewsByClientId = async (clientId) => {
    let reviews = await api.get(
      ApiPaths.GetStylistClientReviewsByClientId(clientId)
    );
    return reviews;
  };

  const updateStylistClientReview = async ({ clientId, text, score }) => {
    const review = await api.post(ApiPaths.UpdateStylistClientReview, {
      clientId,
      text,
      score,
    });
    return review;
  };

  const getClientReviewsLeftByStylist = async (stylistId) => {
    const reviews = await api.get(
      ApiPaths.GetClientReviewsLeftByStylist(stylistId)
    );
    return reviews;
  };

  return {
    createStylistClientReview,
    getStylistClientReviewsByClientId,
    updateStylistClientReview,
    getClientReviewsLeftByStylist,
  };
};

export default useStylistClientReviewService;
