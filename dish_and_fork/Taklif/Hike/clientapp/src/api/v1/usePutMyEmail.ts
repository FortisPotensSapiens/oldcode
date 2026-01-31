import { useMutation, UseMutationResult } from 'react-query';

import { EmailUpdateModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutMyEmail = () => UseMutationResult<string, UseQueryError, EmailUpdateModel, unknown>;

const usePutMyEmail: UsePutMyEmail = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/user-profile/my/email`, data)));
};

export { usePutMyEmail };
export type { UsePutMyEmail };
