import { MouseEventHandler, useCallback } from 'react';

type UseSelect = (onSelect?: (value: number) => void) => MouseEventHandler<HTMLDivElement>;

const useSelect: UseSelect = (onSelect) =>
  useCallback(
    (event) => {
      if (!onSelect) {
        return;
      }

      event.stopPropagation();

      const dataIndex = event.currentTarget.getAttribute('data-index');

      if (!dataIndex) {
        return;
      }

      try {
        onSelect(parseInt(dataIndex, 10));
      } catch (e) {
        console.error(`Can't select image with index "${dataIndex}"`);
      }
    },
    [onSelect],
  );

export { useSelect };
