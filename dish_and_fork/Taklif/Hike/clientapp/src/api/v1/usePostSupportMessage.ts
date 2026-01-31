import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostSupportMessage = () => UseMutationResult<string, unknown, FormData, unknown>;

const usePostSupportMessage: UsePostSupportMessage = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/messaging/send-feedback`, data)));
};

export { usePostSupportMessage };
export type { UsePostSupportMessage };
