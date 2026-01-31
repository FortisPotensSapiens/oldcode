import { useMutation, UseMutationResult } from 'react-query';

import { CheckVerificationCodeModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostCheckVerificationCode = () => UseMutationResult<string, unknown, CheckVerificationCodeModel, unknown>;

const usePostCheckVerificationCode: UsePostCheckVerificationCode = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/messaging/check-verification-code`, data)));
};

export { usePostCheckVerificationCode };
export type { UsePostCheckVerificationCode };
