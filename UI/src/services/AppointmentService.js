import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useAppointmentService = () => {
  const api = useApi();

  const createAppointment = async ({ startDate, endDate, services }) => {
    await api.post(ApiPaths.CreateAppointment, {
      startDate,
      endDate,
      services,
    });
  };

  const getAppointmentsByStylistId = async (stylistId) => {
    const bookedIntervals = await api.get(
      ApiPaths.GetStylistAppointmentsByStylistId(stylistId)
    );
    return bookedIntervals;
  };

  const getAppointmentByStartDateAndStylistId = async (
    stylistId,
    startDate
  ) => {
    const appointment = await api.get(
      ApiPaths.GetAppointmentByStartDateAndStylistId(stylistId, startDate)
    );
    return appointment;
  };

  const acceptAppointment = async (appointmentId) => {
    await api.post(ApiPaths.AcceptAppointment(appointmentId));
  };

  const denyAppointment = async (appointmentId) => {
    await api.post(ApiPaths.DenyAppointment(appointmentId));
  };

  const getClientAllAppointments = async () => {
    var appointments = await api.get(ApiPaths.GetClientAllAppointments);
    return appointments;
  };

  const getStylistAllAppointments = async () => {
    var appointments = await api.get(ApiPaths.GetStylistAllAppointments);
    return appointments;
  };

  return {
    createAppointment,
    getAppointmentsByStylistId,
    getAppointmentByStartDateAndStylistId,
    acceptAppointment,
    denyAppointment,
    getClientAllAppointments,
    getStylistAllAppointments,
  };
};

export default useAppointmentService;
