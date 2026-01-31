import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { FileReadModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetFileById = (
  id: string,
  options?: Omit<UseQueryOptions<FileReadModel | undefined, UseQueryError, FileReadModel>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<FileReadModel | undefined, UseQueryError>;

const useGetFileById: UseGetFileById = (id, options) => {
  const { get } = useAxios();

  return useQuery(
    ['apiV1GetFileById', id],
    (): Promise<FileReadModel | undefined> => {
      if (!id) {
        return Promise.resolve(undefined);
      }

      return extractData(get(`/files/${id}`));
    },
    options,
  );
};

export { useGetFileById };
export type { UseGetFileById };
