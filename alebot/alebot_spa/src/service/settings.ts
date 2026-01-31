import api from "./api";

export const getSettings = async () => {
  return await api
    .get(`/api/v1/sso/users/my-user-info`)
    .then((res) => res.data)
    .catch((e) => console.log(e));
};