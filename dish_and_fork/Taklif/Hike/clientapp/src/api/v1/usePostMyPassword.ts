import { useMutation, UseMutationResult } from 'react-query';

import { PasswordCreateModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostMyPassword = () => UseMutationResult<string, UseQueryError, PasswordCreateModel, unknown>;

const usePostMyPassword: UsePostMyPassword = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/user-profile/my/password`, data)));
};

export { usePostMyPassword };
export type { UsePostMyPassword };
