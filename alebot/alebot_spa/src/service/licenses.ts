import api from "./api";

export const getLicenses = async () => {
  return await api
    .get(`/api/v1/licenses`)
    .then((res) => res.data)
    .catch((e) => console.log(e));
};

export const setAccountForLicense = async (
  id: string,
  tradingAccount: string
) => {
  return await api
    .put(`/api/v1/licenses/${id}`, {
      tradingAccount,
    })
    .then((res) => res)
    .catch((e) => e);
};
