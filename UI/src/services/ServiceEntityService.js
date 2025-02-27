import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useServiceEntityService = () => {
  const api = useApi();

  const getServices = async () => {
    const services = await api.get(ApiPaths.GetServices);
    return services;
  };
  return {
    getServices,
  };
};
export default useServiceEntityService;
