import { useMutation, UseMutationResult } from 'react-query';

import { UseQueryError } from '~/types/query';
import { ApplicationCreateModel } from '~/types/swagger';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostNewIndividualOrder = () => UseMutationResult<string, UseQueryError, ApplicationCreateModel, unknown>;

export const usePostNewIndividualOrder: UsePostNewIndividualOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/individual-applications', data)));
};
