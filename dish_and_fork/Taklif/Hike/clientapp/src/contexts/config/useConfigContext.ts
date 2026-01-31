import { merge } from 'lodash-es';
import { useMemo } from 'react';
import { DeepPartial, DeepReadonly } from 'ts-essentials';

import config, { Config } from '~/config';

const useConfigContext = (value?: DeepPartial<Config>): DeepReadonly<Config> =>
  useMemo(() => (value ? merge({}, config, value) : config), [value]);

export default useConfigContext;
