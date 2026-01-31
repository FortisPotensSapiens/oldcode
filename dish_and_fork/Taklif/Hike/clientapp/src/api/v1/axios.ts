import { useOidcAccessToken } from '@axa-fr/react-oidc';
import Axios, { AxiosError, AxiosInstance } from 'axios';
import constate from 'constate';
import { merge } from 'lodash-es';
import { useMemo } from 'react';

import { useConfig } from '~/contexts';

type UseAxiosContext = () => AxiosInstance;

const useAxiosContext: UseAxiosContext = () => {
  const token = useOidcAccessToken();
  const { api } = useConfig();
  const { baseURL } = api;

  return useMemo(() => {
    const instance = Axios.create({ baseURL });

    instance.interceptors.request.use((config) => {
      if (!token.accessToken) {
        return config;
      }

      return merge({}, config, {
        headers: {
          Authorization: `Bearer ${token.accessToken}`,
        },
      });
    });

    // eslint-disable-next-line consistent-return
    instance.interceptors.response.use(undefined, async (error: AxiosError) => {
      const { response } = error;

      if (response?.status !== 403) {
        return Promise.reject(error);
      }
    });

    return instance;
  }, [baseURL, token.accessToken]);
};

const [ApiV1Provider, useAxios] = constate(useAxiosContext);

export { ApiV1Provider, useAxios };
