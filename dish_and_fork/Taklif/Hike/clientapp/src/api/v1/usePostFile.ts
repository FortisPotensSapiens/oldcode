import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostFile = () => UseMutationResult<string, unknown, FormData, unknown>;

const usePostFile: UsePostFile = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/files`, data)));
};

export { usePostFile };
export type { UsePostFile };
