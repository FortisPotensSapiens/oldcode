import { useMutation, UseMutationResult } from 'react-query';

import { UseQueryError, UserProfileUpdateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutMyUserProfile = () => UseMutationResult<string, UseQueryError, UserProfileUpdateModel, unknown>;

const usePutMyUserProfile: UsePutMyUserProfile = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/user-profile/my`, data)));
};

export { usePutMyUserProfile };
export type { UsePutMyUserProfile };
