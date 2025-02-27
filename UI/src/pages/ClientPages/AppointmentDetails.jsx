import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import useAppointmentService from "../../services/AppointmentService";

const AppointmentDetails = () => {
  const [appointments, setAppointments] = useState(undefined);
  const [appointmentsAreLoading, setAppointmentsAreLoading] = useState(true);
  const { getClientAllAppointments } = useAppointmentService();
  const navigate = useNavigate();

  const fetchAppointments = async () => {
    setAppointmentsAreLoading(true);
    try {
      let appointments = await getClientAllAppointments();
      setAppointments(appointments);
      setAppointmentsAreLoading(false);
    } catch (e) {
      setAppointmentsAreLoading(false);
    }
  };

  useEffect(() => {
    fetchAppointments();
  }, []);

  return (
    <>
      {" "}
      <div>
        <h1>Future appointments</h1>
        {!appointmentsAreLoading && (
          <div className="appointment-grid">
            {appointments
              .filter(
                (appointment) => new Date() < new Date(appointment.startDate)
              )
              .map(
                (appointment) =>
                  (appointment.status == "Accepted" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px green" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        {appointment.status}
                      </div>
                    </div>
                  )) ||
                  (appointment.status == "Denied" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px red" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        {appointment.status}
                      </div>
                    </div>
                  )) ||
                  (appointment.status == "Pending" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px gray" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        {appointment.status}
                      </div>
                    </div>
                  ))
              )}
          </div>
        )}
        <h1>Past appointments</h1>
        {!appointmentsAreLoading && (
          <div className="appointment-grid">
            {appointments
              .filter(
                (appointment) => new Date() >= new Date(appointment.startDate)
              )
              .map(
                (appointment) =>
                  (appointment.status == "Accepted" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px green" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        Status: {appointment.status}
                      </div>
                    </div>
                  )) ||
                  (appointment.status == "Denied" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px red" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        Status: {appointment.status}
                      </div>
                    </div>
                  )) ||
                  (appointment.status == "Pending" && (
                    <div key={appointment.id}>
                      <div
                        className="all-appointment-details"
                        style={{ boxShadow: "4px 4px 4px 4px gray" }}
                        onClick={() =>
                          navigate(
                            `/stylists/${appointment.services[0].stylistId}`
                          )
                        }
                      >
                        Appointment made for:
                        {appointment.startDate} {""}
                        Services:{""}
                        {appointment.services.map((service) => (
                          <div key={service.id}>{service.service}</div>
                        ))}
                        Status: {appointment.status}
                      </div>
                    </div>
                  ))
              )}
          </div>
        )}
      </div>{" "}
      <div></div>
    </>
  );
};

export default AppointmentDetails;
