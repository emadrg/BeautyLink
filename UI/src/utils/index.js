export const getFile = (path) => {
  return `${import.meta.env.VITE_API_URL}/files/${path}`;
};

export const jsonToFormData = (object) => {
  const formData = new FormData();
  Object.keys(object).forEach((key) => {
    if (Array.prototype === object[key].__proto__) {
      object[key].forEach((element) => {
        formData.append(key, element);
      });
    } else {
      if (object[key]) {
        formData.append(key, object[key]);
      }
    }
  });
  return formData;
};
