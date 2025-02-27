import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useClientSalonReviewService = () => {
  const api = useApi();

  const getClientSalonReviewsBySalonId = async (salonId, skip, take) => {
    const reviews = await api.get(
      ApiPaths.GetClientSalonReviewsBySalonId(salonId, skip, take)
    );
    return reviews;
  };

  const createClientSalonReview = async ({ salonId, text, score }) => {
    await api.post(ApiPaths.CreateClientSalonReview, {
      salonId,
      text,
      score,
    });
  };

  const updateClientSalonReview = async ({ salonId, text, score }) => {
    const review = await api.post(ApiPaths.UpdateClientSalonReview, {
      salonId,
      text,
      score,
    });
    return review;
  };

  return {
    getClientSalonReviewsBySalonId,
    createClientSalonReview,
    updateClientSalonReview,
  };
};

export default useClientSalonReviewService;
