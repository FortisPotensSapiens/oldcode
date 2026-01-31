import { useMutation, UseMutationResult } from 'react-query';

import { AddLoginModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseAddMyExternalLogin = () => UseMutationResult<string, UseQueryError, AddLoginModel, unknown>;

const useAddMyExternalLogin: UseAddMyExternalLogin = () => {
  const { post } = useAxios();

  return useMutation((data) =>
    extractData(
      post(`/user-profile/my/external-login`, data).then((data) => {
        window.location.assign(data.data);
        return data;
      }),
    ),
  );
};

export { useAddMyExternalLogin };
export type { UseAddMyExternalLogin };
