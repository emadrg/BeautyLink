import { Stack } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import "../../../src/styles/pages/clientHomePage.scss";
import CustomLoader from "../../components/utility/CustomLoader";
import { Select } from "../../components/widgets/Select";
import resources from "../../resources/resources";
import useSalonService from "../../services/SalonService";
import useServiceEntityService from "../../services/ServiceEntityService";
import Salon from "./SalonListItem";

const Salons = () => {
  const [salonList, setSalonList] = useState([]);
  const [service, setService] = useState("");
  const [serviceList, setserviceList] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [listLength, setListLength] = useState(undefined);
  const [displayLoadMoreButton, setDisplayLoadMoreButton] = useState(true);
  const pageSize = 3;
  const { getSalons } = useSalonService();
  const { getServices } = useServiceEntityService();

  const { t } = useTranslation();

  const fetchSalons = async (skip, take) => {
    try {
      const salons = await getSalons(service, skip, take);
      if (listLength < salons.length || listLength == undefined) {
        setSalonList(salons);
        setIsLoading(false);
        setListLength(salons.length);
      }

      if (listLength >= salons.length && listLength != undefined) {
        setDisplayLoadMoreButton(false);
        setIsLoading(false);
        setListLength(salons.length);
      }
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchServices = async () => {
    setIsLoading(true);
    try {
      const services = await getServices();
      setserviceList(services);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const getSalonsByService = async (serviceId, skip, take) => {
    const salons = await getSalons(serviceId, skip, take);
    setSalonList(salons);
  };

  useEffect(() => {
    fetchSalons(0, 3);
    fetchServices();
  }, []);

  useEffect(() => {
    fetchSalons(0, currentPage * 3);
  }, [currentPage]);

  if (isLoading) {
    return <CustomLoader />;
  }

  const handleServiceChange = async (event, skip, take) => {
    setService(event.target.value);
    await getSalonsByService(event.target.value, skip, take);
  };

  const handleLoadMore = () => {
    let localCurrentPage = currentPage;
    setCurrentPage(localCurrentPage + 1);
  };

  return (
    <div>
      {!isLoading && (
        <Stack gap={8}>
          <div>
            <Select
              options={serviceList}
              fullWidth={true}
              label={"Service"}
              onChange={(e) => handleServiceChange(e, 0, 3)}
              sx={{ minWidth: 100 }}
            ></Select>
            <div>
              <h1>{t(resources.allSalons.title)}</h1>

              <div className="salon-list-all-salons">
                {salonList.map((salon) => {
                  return <Salon key={salon.id} {...salon} />;
                })}
              </div>
              {displayLoadMoreButton && (
                <button onClick={() => handleLoadMore()}>Load more</button>
              )}
            </div>
          </div>
        </Stack>
      )}
    </div>
  );
};

export default Salons;
