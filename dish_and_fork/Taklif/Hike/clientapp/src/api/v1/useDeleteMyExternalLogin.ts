import { useMutation, UseMutationResult } from 'react-query';

import { LoginRemoveModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteMyExternalLogin = () => UseMutationResult<string, UseQueryError, LoginRemoveModel, unknown>;

const useDeleteMyExternalLogin: UseDeleteMyExternalLogin = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/user-profile/my/external-login`, { data })));
};

export { useDeleteMyExternalLogin };
export type { UseDeleteMyExternalLogin };
