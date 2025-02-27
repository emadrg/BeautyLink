import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { roleTypes } from "../../constants";
import useClientSalonReviewService from "../../services/ClientSalonReviewService";
import useManagerService from "../../services/ManagerService";
import useSalonService from "../../services/SalonService";
import userSession from "../../utils/userSession";
import DisplayAllReviews from "../Stylist/StylistAllReviews";

const SalonAllReviews = () => {
  const user = userSession.user();
  const [reviews, setReviews] = useState(undefined);
  const [salon, setSalon] = useState(undefined);
  const [currentPage, setCurrentPage] = useState(1);
  const [salonIsLoading, setSalonIsLoading] = useState(true);
  const [reviewsAreLoading, setReviewsAreLoading] = useState(true);
  const { getSalonIdByManagerId, getSalonWithDetailsById } = useSalonService();
  const pageSize = 3;
  const { getClientSalonReviewsBySalonId } = useClientSalonReviewService();
  const { getManagerIdByUserId } = useManagerService();
  const navigate = useNavigate();

  const fetchReviews = async (skip, take) => {
    try {
      const managerId = await getManagerIdByUserId(user.id);
      const salonId = await getSalonIdByManagerId(managerId);
      const reviews = await getClientSalonReviewsBySalonId(salonId, skip, take);
      setReviews(reviews);
      setReviewsAreLoading(false);
    } catch (e) {
      setReviewsAreLoading(false);
    }
  };

  const fetchSalon = async () => {
    try {
      const managerId = await getManagerIdByUserId(user.id);
      const salonId = await getSalonIdByManagerId(managerId);
      setSalonIsLoading(false);
      const salon = await getSalonWithDetailsById(salonId);
      setSalon(salon);
    } catch (e) {
      setSalonIsLoading(false);
    }
  };

  const handleLoadMore = () => {
    let localCurrentPage = currentPage;
    setCurrentPage(localCurrentPage + pageSize);
  };

  useEffect(() => {
    fetchReviews(0, pageSize);
    fetchSalon();
  }, []);

  useEffect(() => {
    if (user == null || user.roleId != roleTypes.managerId) {
      navigate("/login");
    } else {
      fetchReviews(0, currentPage * pageSize);
    }
  }, [currentPage]);

  return (
    <div>
      {user && user.roleId == roleTypes.managerId && (
        <div>
          {salon && <h1>Reviews for {salon.salonName}</h1>}

          {reviews && reviews.length > 0 && (
            <div>
              <div>
                {reviews.map((review) => {
                  return <DisplayAllReviews key={review.id} {...review} />;
                })}
              </div>
              <button onClick={() => handleLoadMore()}>Load more</button>
            </div>
          )}

          {reviews && reviews.length == 0 && <div> No reviews yet </div>}
        </div>
      )}
    </div>
  );
};

export default SalonAllReviews;
