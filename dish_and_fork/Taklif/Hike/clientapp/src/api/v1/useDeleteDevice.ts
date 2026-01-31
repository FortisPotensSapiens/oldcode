import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteDevice = () => UseMutationResult<string, unknown, string>;

const useDeleteDevice: UseDeleteDevice = () => {
  const ax = useAxios();

  return useMutation((id) => extractData(ax.delete(`/devices/${id}`)));
};

export { useDeleteDevice };
export type { UseDeleteDevice };
