import { useState } from "react";
import { useNavigate } from "react-router-dom";

const ServiceWithStylists = (props) => {
  const [showStylist, setShowStylist] = useState(false);
  const { stylists, service } = props;
  const navigate = useNavigate();
  return (
    <div style={{ display: "flex" }}>
      <button onClick={() => setShowStylist(!showStylist)}>{service}</button>
      {showStylist && (
        <p className="service-stylists">
          {stylists.map((stylist) => (
            <button
              onClick={() => navigate(`/stylists/${stylist.stylistId}`)}
              key={stylist.stylistId}
            >
              {stylist.stylist}
            </button>
          ))}
        </p>
      )}
    </div>
  );
};

export default ServiceWithStylists;
