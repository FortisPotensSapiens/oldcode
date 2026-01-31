import { useQuery, UseQueryResult } from 'react-query';

import { DeviceReadModelPageResultModel, PaginationModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetMyDevices = (payload: PaginationModel, enabled: boolean) => UseQueryResult<DeviceReadModelPageResultModel>;

const useGetMyDevices: UseGetMyDevices = (payload: PaginationModel, enabled = false) => {
  const { get } = useAxios();

  return useQuery(
    ['useGetMyDevices', payload.pageNumber, payload.pageSize],
    () => extractData(get(`/devices/my?PageNumber=${payload.pageNumber}&PageSize=${payload.pageSize}`)),
    {
      enabled,
    },
  );
};

export { useGetMyDevices };
export type { UseGetMyDevices };
