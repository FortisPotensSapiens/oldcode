import constate from 'constate';

import useConfigContext from './useConfigContext';

const [ConfigProvider, useConfig] = constate(useConfigContext);

export type { Config } from '~/config';
export { ConfigProvider, useConfig };
