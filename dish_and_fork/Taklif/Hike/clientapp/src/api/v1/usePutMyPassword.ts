import { useMutation, UseMutationResult } from 'react-query';

import { PasswordCreateModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutMyPassword = () => UseMutationResult<string, UseQueryError, PasswordCreateModel, unknown>;

const usePutMyPassword: UsePutMyPassword = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/user-profile/my/password`, data)));
};

export { usePutMyPassword };
export type { UsePutMyPassword };
