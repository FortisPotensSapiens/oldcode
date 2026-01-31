import api from "./api";

export const getServers = async () => {
  return await api
    .get(`/api/v1/vds-servers`)
    .then((res) => res.data)
    .catch((e) => console.log(e));
};