import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useStylistService = () => {
  const api = useApi();
  const getStylistById = async (id) => {
    const stylist = await api.get(ApiPaths.GetStylistById(id));
    return stylist;
  };

  const getStylistWithServices = async (id) => {
    const stylistWithServices = await api.get(
      ApiPaths.GetStylistWithServices(id)
    );
    return stylistWithServices;
  };

  const getStylistProfilePicture = async (id) => {
    const picture = await api.get(ApiPaths.GetProfilePictureName(id));
    return picture;
  };

  const getStylistAppointments = async (id) => {
    const appointments = await api.get(
      ApiPaths.GetStylistAllAppointmentsByStylistId(id)
    );
    return appointments;
  };

  const getStylistIdByUserId = async (userId) => {
    const stylistId = await api.get(ApiPaths.GetStylistIdByUserId(userId));
    return stylistId;
  };

  const hasClientVisitedStylist = async (stylistId) => {
    const response = await api.get(ApiPaths.HasClientVisitedStylist(stylistId));
    return response;
  };

  const getStylistByFiltering = async (
    startDate,
    endDate,
    serviceId,
    cityId
  ) => {
    const stylists = await api.get(
      ApiPaths.GetStylistByFiltering(startDate, endDate, serviceId, cityId)
    );
    return stylists;
  };

  return {
    getStylistById,
    getStylistWithServices,
    getStylistProfilePicture,
    getStylistAppointments,
    getStylistIdByUserId,
    hasClientVisitedStylist,
    getStylistByFiltering,
  };
};

export default useStylistService;
