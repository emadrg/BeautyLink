import { TextField } from "@mui/material";
import { useState } from "react";
import useAdminService from "../../services/AdminService";

const UpdateUserDetails = (existingUserObject) => {
  const [updatedUser, setUpdatedUser] = useState({ ...existingUserObject });
  const [isEditable, setIsEditable] = useState(false);
  const { updateUser } = useAdminService();

  const handleUpdateUser = async () => {
    let userObject = {
      guidId: updatedUser.existingUserObject.guidId,
      firstName: updatedUser.firstName,
      lastName: updatedUser.lastName,
      email: updatedUser.email,
      roleId: updatedUser.roleId,
      statusId: updatedUser.statusId,
    };
    debugger;
    try {
      await updateUser(userObject);
      //handleModalState();
    } catch (err) {
      console.log(err);
    }
  };

  const handleCancel = () => {
    setUpdatedUser({ ...existingUserObject });
    setIsEditable(false);
  };

  return (
    <div>
      <TextField
        style={{ width: 200 }}
        className="review-text-field"
        label="First Name"
        value={updatedUser.firstName}
        onChange={(event) => {
          setUpdatedUser({ ...updatedUser, firstName: event.target.value });
        }}
        maxRows={4}
        disabled={!isEditable}
      />
      <TextField
        style={{ width: 200 }}
        className="review-text-field"
        label="Last Name"
        value={updatedUser.lastName}
        onChange={(event) => {
          setUpdatedUser({ ...updatedUser, lastName: event.target.value });
        }}
        maxRows={4}
        disabled={!isEditable}
      />
      <TextField
        style={{ width: 200 }}
        className="review-text-field"
        label="Email"
        value={updatedUser.email}
        onChange={(event) => {
          setUpdatedUser({ ...updatedUser, email: event.target.value });
        }}
        maxRows={4}
        disabled={!isEditable}
      />
      <TextField
        style={{ width: 200 }}
        className="review-text-field"
        label="Role Id"
        value={updatedUser.roleId}
        onChange={(event) => {
          setUpdatedUser({ ...updatedUser, roleId: event.target.value });
        }}
        maxRows={4}
        disabled={!isEditable}
      />
      <TextField
        style={{ width: 200 }}
        className="review-text-field"
        label="Status Id"
        value={updatedUser.statusId}
        onChange={(event) => {
          setUpdatedUser({ ...updatedUser, statusId: event.target.value });
        }}
        maxRows={4}
        disabled={!isEditable}
      />
      {!isEditable && (
        <button onClick={() => setIsEditable(true)}>Update</button>
      )}
      {isEditable && (
        <>
          <button onClick={handleUpdateUser}>Save</button>
          <button onClick={handleCancel}>Cancel</button>
        </>
      )}
    </div>
  );
};

export default UpdateUserDetails;
