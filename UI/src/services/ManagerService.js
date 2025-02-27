import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useManagerService = () => {
  const api = useApi();

  const getStylistsByManagerId = async (managerId) => {
    const stylists = await api.get(ApiPaths.GetStylistsByManagerId(managerId));
    return stylists;
  };

  const getManagerIdByUserId = async (userId) => {
    const managerId = await api.get(ApiPaths.GetManagerIdByUserId(userId));
    return managerId;
  };

  const getStylistsReviewsByManagerId = async (managerId, skip, take) => {
    const reviews = await api.get(
      // ApiPaths.GetStylistsReviewsByManagerId(managerId, skip, take)
      ApiPaths.GetStylistsReviewsByManagerId(managerId)
    );
    return reviews;
  };

  return {
    getStylistsByManagerId,
    getManagerIdByUserId,
    getStylistsReviewsByManagerId,
  };
};

export default useManagerService;
