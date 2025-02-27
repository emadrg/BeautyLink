import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useUnavailableTimeServce = () => {
  const api = useApi();

  const getUnavailableTimeByStylistId = async (stylistId) => {
    const unavailableTime = await api.get(
      ApiPaths.GetUnavailableTimeByStylistId(stylistId)
    );
    return unavailableTime;
  };

  const createUnavailableTime = async (stylistId, unavailableTimeObject) => {
    return await api.post(
      ApiPaths.createUnavailableTime(stylistId),
      unavailableTimeObject
    );
  };

  return {
    getUnavailableTimeByStylistId,
    createUnavailableTime,
  };
};

export default useUnavailableTimeServce;
