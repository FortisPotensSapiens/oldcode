import { useMutation, UseMutationResult } from 'react-query';

import { SendVerificationSmsModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostSendVerificationCode = () => UseMutationResult<string, unknown, SendVerificationSmsModel, unknown>;

const usePostSendVerificationCode: UsePostSendVerificationCode = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/messaging/send-verification-code`, data)));
};

export { usePostSendVerificationCode };
export type { UsePostSendVerificationCode };
