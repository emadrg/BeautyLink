import { useTranslation } from "react-i18next";

const Service = ({
  serviceId,
  serviceName,
  durationMinutes,
  price,
  handleAddService,
  isSelected,
}) => {
  const { t } = useTranslation();
  return (
    <div
      className="service-app"
      style={{
        backgroundColor: isSelected ? "#e3c3d2" : undefined,
      }}
      onClick={handleAddService}
    >
      {" "}
      <div className = "service-name">{serviceName} </div>
      <i className = "service-details">
        {durationMinutes} minutes, {price} lei
      </i>
    </div>
  );
};

export default Service;
