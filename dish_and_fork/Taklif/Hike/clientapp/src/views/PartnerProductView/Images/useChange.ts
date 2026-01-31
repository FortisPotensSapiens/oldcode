import { useCallback } from 'react';

import { FileReadModel } from '~/types';

import { ChangeHandler, ImageValue } from './types';

type UseChange = (value: ImageValue[], onChange: ChangeHandler) => (image: FileReadModel) => void;

const useChange: UseChange = (value, onChange) =>
  useCallback(
    (image) => {
      const index = value.findIndex((item) => (typeof item === 'string' ? item : item.id) === image.id);

      if (index === -1) {
        console.error(`Can't find image with id ${image.id}`, image);
        return;
      }

      const changed = [...value];
      changed[index] = image;
      onChange(changed);
    },
    [value, onChange],
  );

export { useChange };
export type { UseChange };
