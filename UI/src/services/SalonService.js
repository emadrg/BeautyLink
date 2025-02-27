import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useSalonService = () => {
  const api = useApi();

  const getSalons = async (serviceId, skip, take) => {
    const salons = await api.get(ApiPaths.GetSalons(serviceId, skip, take));
    return salons;
  };

  const getSalonWithDetailsById = async (id) => {
    const salon = await api.get(ApiPaths.GetSalonWithDetails(id));
    return salon;
  };

  const getSalonsByCityIdAndServiceId = async (cityId, serviceId) => {
    const salons = await api.get(
      ApiPaths.GetSalonsByCityIdAndServiceId(cityId, serviceId)
    );
    return salons;
  };

  const createSalon = async ({
    name,
    countyId,
    cityId,
    address,
    latitude,
    longitude,
  }) => {
    await api.post(ApiPaths.CreateSalon, {
      name,
      countyId,
      cityId,
      address,
      latitude,
      longitude,
    });
  };

  const getSalonPictures = async (salonId, skip, take) => {
    const pictures = await api.get(
      ApiPaths.GetSalonPictures(salonId, skip, take)
    );
    return pictures;
  };

  const getLastVisitedSalonByClientId = async (clientId) => {
    try {
      const salon = await api.get(
        ApiPaths.GetLastVisitedSalonByClientId(clientId)
      );
      return salon;
    } catch (ex) {
      console.log(ex);
    }
    return null;
  };

  const getSalonSuggestions = async (cityId) => {
    const suggestions = await api.get(ApiPaths.GetSalonSuggestions(cityId));
    return suggestions;
  };

  const getSalonIdByManagerId = async (managerId) => {
    const salonId = await api.get(ApiPaths.GetSalonIdByManagerId(managerId));
    return salonId;
  };

  const hasClientVisitedSalon = async (salonId) => {
    const response = await api.get(ApiPaths.HasClientVisitedSalon(salonId));
    return response;
  };

  return {
    getSalons,
    getSalonWithDetailsById,
    createSalon,
    getSalonPictures,
    getSalonsByCityIdAndServiceId,
    getLastVisitedSalonByClientId,
    getSalonSuggestions,
    getSalonIdByManagerId,
    hasClientVisitedSalon,
  };
};

export default useSalonService;
