import { TextField } from "@mui/material";
import { useState } from "react";
import useServiceStylistService from "../../services/ServiceStylistService";
import useStylistService from "../../services/StylistService";
import userSession from "../../utils/userSession";

const UpdateService = (existingServiceObject) => {
  const [updatedService, setUpdatedService] = useState({
    ...existingServiceObject,
  });
  const user = userSession.user();
  const { updateServiceForCurrentStylist } = useServiceStylistService();
  const { getStylistIdByUserId } = useStylistService();

  const handleUpdateService = async () => {
    let serviceObject = {
      serviceId: existingServiceObject.existingServiceObject.serviceId,
      stylistId: await getStylistIdByUserId(user.id),
      durationMinutes: updatedService.durationMinutes,
      price: updatedService.price,
    };
    try {
      await updateServiceForCurrentStylist(serviceObject);
    } catch (err) {
      console.log(err);
    }
  };

  const handleCancel = () => {
    setUpdatedService({ ...existingServiceObject });
  };

  return (
    <div>
      <TextField
        label="Price"
        value={updatedService.price}
        onChange={(event) => {
          setUpdatedService({ ...updatedService, price: event.target.value });
        }}
        maxRows={4}
      />

      <TextField
        label="Time"
        value={updatedService.durationMinutes}
        onChange={(event) => {
          setUpdatedService({
            ...updatedService,
            durationMinutes: event.target.value,
          });
        }}
        maxRows={4}
      />

      <>
        <button onClick={handleUpdateService}>Save</button>
        <button onClick={handleCancel}>Cancel</button>
      </>
    </div>
  );
};

export default UpdateService;
