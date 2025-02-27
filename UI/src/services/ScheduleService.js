import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useScheduleService = () => {
  const api = useApi();

  const getScheduleByStylistId = async (stylistId) => {
    const schedule = await api.get(ApiPaths.GetScheduleByStylisId(stylistId));
    return schedule;
  };

  const getWeekdays = async () => {
    const weekdays = await api.get(ApiPaths.GetWeekdays);
    return weekdays;
  };

  const createOrUpdateSchedule = async (stylistId, scheduleObject) => {
    return await api.post(
      ApiPaths.CreateOrUpdateSchedule(stylistId),
      scheduleObject
    );
  };

  const deleteSchedule = async (stylistId, weekdayId) => {
    return await api.post(ApiPaths.DeleteSchedule(stylistId, weekdayId));
  };

  return {
    getScheduleByStylistId,
    getWeekdays,
    createOrUpdateSchedule,
    deleteSchedule,
  };
};

export default useScheduleService;
