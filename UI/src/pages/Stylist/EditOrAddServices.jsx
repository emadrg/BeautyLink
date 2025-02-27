import { Box, Modal, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { Select } from "../../components/widgets/Select";
import useServiceEntityService from "../../services/ServiceEntityService";
import useServiceStylistService from "../../services/ServiceStylistService";
import useStylistService from "../../services/StylistService";
import userSession from "../../utils/userSession";
import UpdateService from "./UpdateService";

const EditOrAddServices = () => {
  const [currentServices, setCurrentServices] = useState(undefined);
  const [currentServicesAreLoading, setCurrentServicesAreLoading] =
    useState(true);
  const [editButtonIsPressed, setEditButtonIsPressed] = useState([]);
  const [serviceList, setServiceList] = useState([]);
  const [addServiceButtonIsClicked, setAddServiceButtonIsClicked] =
    useState(false);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [selectedService, setSelectedService] = useState(undefined);
  const [newServiceIsSelected, setNewServiceIsSelected] = useState(false);
  const [newServiceTime, setNewServiceTime] = useState(undefined);
  const [newServicePrice, setNewServicePrice] = useState(undefined);

  const user = userSession.user();

  const {
    getServicesForCurrentStylist,
    createServiceForCurrentStylist,
    deleteServiceForCurrentStylist,
  } = useServiceStylistService();

  const { getServices } = useServiceEntityService();
  const { getStylistIdByUserId } = useStylistService();

  const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 350,
    height: 150,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
    paddingLeft: 5,
  };

  const fetchCurrentServices = async () => {
    try {
      let currentServices = await getServicesForCurrentStylist();
      setCurrentServices(currentServices);
      let editButtonIsPressed = [];

      let counter = 0;
      currentServices.forEach((service) => {
        editButtonIsPressed[counter] = false;
        service.id = counter++;
      });

      setEditButtonIsPressed(editButtonIsPressed);
      setCurrentServicesAreLoading(false);
    } catch (e) {
      setCurrentServicesAreLoading(false);
    }
  };

  const fetchServices = async () => {
    const services = await getServices();
    setServiceList(services);
  };

  const handleDelete = async (serviceObj) => {
    const serviceId = serviceObj.service.serviceId;
    await deleteServiceForCurrentStylist(serviceId);
  };

  const handleNewServiceSelect = (event) => {
    setSelectedService(event.target.value);
    setNewServiceIsSelected(true);
  };

  const handleAddServiceIsPressed = () => {
    setAddServiceButtonIsClicked(!addServiceButtonIsClicked);
    setNewServiceIsSelected(false);
  };

  const handleAddService = async (e) => {
    const stylistId = await getStylistIdByUserId(user.id);
    try {
      await createServiceForCurrentStylist({
        serviceId: selectedService,
        stylistId: stylistId,
        durationMinutes: newServiceTime,
        price: newServicePrice,
      });
      setModalIsOpen(!modalIsOpen);
    } catch (err) {
      console.log(err);
    }
  };

  useEffect(() => {
    fetchCurrentServices();
    fetchServices();
  }, []);



  return (
    <div>
      <div>
        Current services:
        {!currentServicesAreLoading && (
          <>
            {currentServices.map((service) => (
              <>
                <div key={service.id}>
                  {service.service}, {service.durationMinutes}, {service.price}
                  <button
                    onClick={() => {
                      setEditButtonIsPressed([
                        ...editButtonIsPressed.slice(0, service.id),
                        {
                          id: service.id,
                          name: !editButtonIsPressed[service.id],
                        },
                        ...editButtonIsPressed.slice(service.id + 1),
                      ]);
                    }}
                  >
                    Edit
                  </button>
                  {editButtonIsPressed[service.id] && (
                    <UpdateService existingServiceObject={service} />
                  )}
                  <button onClick={() => handleDelete({ service })}>
                    Delete
                  </button>
                </div>
              </>
            ))}
          </>
        )}
        <button onClick={handleAddServiceIsPressed}>Add new service</button>
        {addServiceButtonIsClicked && (
          <>
            <Select
              options={serviceList}
              value={selectedService}
              label="services"
              onChange={handleNewServiceSelect}
            ></Select>
            {newServiceIsSelected && (
              <>
                <TextField
                  type="number"
                  label={"Price"}
                  variant="standard"
                  name="email"
                  value={newServicePrice}
                  onChange={(e) => setNewServicePrice(e.target.value)}
                ></TextField>

                <TextField
                  type="number"
                  label={"Time"}
                  variant="standard"
                  name="email"
                  value={newServiceTime}
                  onChange={(e) => setNewServiceTime(e.target.value)}
                ></TextField>

                <button onClick={handleAddService}>Add service</button>
              </>
            )}

            <div>
              <Modal
                open={modalIsOpen}
                onClose={() => setModalIsOpen(!modalIsOpen)}
              >
                <Box sx={{ ...style }}>
                  <div>Service successfully added!</div>
                  <button
                    style={{ marginLeft: 90, marginTop: 20 }}
                    onClick={() => setModalIsOpen(!modalIsOpen)}
                  >
                    ok
                  </button>
                </Box>
              </Modal>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default EditOrAddServices;
