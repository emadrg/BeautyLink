import { useEffect, useState } from "react";
import { Select } from "../../components/widgets/Select";
import useAdminService from "../../services/AdminService";
import useRoleService from "../../services/RoleService";
import "../../styles/pages/admin.scss";
import UpdateUserDetails from "../Admin/UpdateUserDetails";

const AdminHomePage = () => {
  const [users, setUsers] = useState(undefined);
  const [roles, setRoles] = useState(undefined);
  const [selectedRole, setSelectedRole] = useState("");
  const [usersAreLoading, setUsersAreLoading] = useState(true);
  const [rolesAreLoading, setRolesAreLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [editButtonIsPressed, setEditButtonIsPressed] = useState([]);

  const { getAllUsers } = useAdminService();
  const { getRoles } = useRoleService();

  const fetchRoles = async () => {
    try {
      let roles = await getRoles();
      setRoles(roles);
      setRolesAreLoading(false);
    } catch (e) {
      setRolesAreLoading(false);
    }
  };

  const fetchUsers = async (skip, take) => {
    try {
      let users = await getAllUsers(null, skip, take);
      setUsers(users);
      let counter = 0;
      users.forEach((user) => {
        editButtonIsPressed[counter] = false;
        user.id = counter++;
      });

      setEditButtonIsPressed(editButtonIsPressed);
      setUsersAreLoading(false);
    } catch (e) {
      setUsersAreLoading(false);
    }
  };

  const fetchUserByRole = async (selectedRole, skip, take) => {
    try {
      let users = await getAllUsers(selectedRole, skip, take);
      setUsers(users);
      let counter = 0;
      users.forEach((user) => {
        editButtonIsPressed[counter] = false;
        user.id = counter++;
      });

      setEditButtonIsPressed(editButtonIsPressed);
      setUsersAreLoading(false);
    } catch (e) {
      setUsersAreLoading(false);
    }
  };

  const handleRoleSelect = async (event, skip, take) => {
    setSelectedRole(event.target.value);
    await fetchUserByRole(event.target.value, skip, take);
  };

  const handleLoadMore = () => {
    let localCurrentPage = currentPage;
    setCurrentPage(localCurrentPage + 1);
  };

  useEffect(() => {
    fetchRoles();
    fetchUsers(0, currentPage * 10);
  }, []);

  useEffect(() => {
    fetchUsers(0, currentPage * 10);
  }, [currentPage]);

  return (
    <div>
      <div>
        {!rolesAreLoading && (
          <Select
            options={roles}
            label={"Filter by user role"}
            onChange={(e) => handleRoleSelect(e, 0, 10)}
            sx={{ minWidth: 300 }}
          ></Select>
        )}
        {!usersAreLoading &&
          !selectedRole &&
          users.map((user) => (
            <div key={user.id} className="user-details">
              {user.guidId}|{user.firstName}|{user.lastName}|{user.email}|
              {user.roleId}|{user.statusId}
              <button
                onClick={() => {
                  setEditButtonIsPressed([
                    ...editButtonIsPressed.slice(0, user.id),
                    {
                      id: user.id,
                      name: !editButtonIsPressed[user.id],
                    },
                    ...editButtonIsPressed.slice(user.id + 1),
                  ]);
                }}
              >
                Edit
              </button>
              {editButtonIsPressed[user.id] && (
                <UpdateUserDetails existingUserObject={user} />
              )}
            </div>
          ))}

        {!usersAreLoading &&
          selectedRole &&
          users.map((user) => (
            <div key={user.id} className="user-details">
              {user.id}|{user.firstName}|{user.lastName}|{user.email}|
              {user.roleId}|{user.statusId}
              <button
                onClick={() => {
                  setEditButtonIsPressed([
                    ...editButtonIsPressed.slice(0, user.id),
                    {
                      id: user.id,
                      name: !editButtonIsPressed[user.id],
                    },
                    ...editButtonIsPressed.slice(user.id + 1),
                  ]);
                }}
              >
                Edit
              </button>
              {editButtonIsPressed[user.id] && (
                <UpdateUserDetails existingUserObject={user} />
              )}
            </div>
          ))}

        <button onClick={() => handleLoadMore()}>Load more</button>
      </div>
    </div>
  );
};

export default AdminHomePage;
