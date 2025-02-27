import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useServiceStylistService = () => {
  const api = useApi();

  const getServicesForCurrentStylist = async () => {
    const services = await api.get(ApiPaths.GetServicesForCurrentStylist);
    return services;
  };

  const updateServiceForCurrentStylist = async (serviceStylistObject) => {
    return await api.post(
      ApiPaths.UpdateServiceForCurrentStylist,
      serviceStylistObject
    );
  };

  const createServiceForCurrentStylist = async (serviceStylistObject) => {
    return await api.post(
      ApiPaths.CreateServiceForCurrentStylist,
      serviceStylistObject
    );
  };

  const deleteServiceForCurrentStylist = async (id) => {
    return await api.delete(ApiPaths.DeleteServiceForCurrentStylist(id));
  };

  return {
    getServicesForCurrentStylist,
    updateServiceForCurrentStylist,
    createServiceForCurrentStylist,
    deleteServiceForCurrentStylist,
  };
};

export default useServiceStylistService;
