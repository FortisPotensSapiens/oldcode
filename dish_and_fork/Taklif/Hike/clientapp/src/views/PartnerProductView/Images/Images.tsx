import { Box, BoxProps } from '@mui/material';
import { Modal } from 'antd';
import { FC, MouseEventHandler, useState } from 'react';

import { UploadImage } from '~/components';
import { useConfig } from '~/contexts';
import { FileReadModel } from '~/types';

import { Image } from './Image';
import { ChangeHandler, ImageValue } from './types';
import { useAdd } from './useAdd';
import { useChange } from './useChange';
import { useSelect } from './useSelect';

type ImagesProps = {
  disabled?: boolean;
  showCrow?: boolean;
  addabled?: boolean;
  max?: number;
  onChange?: ChangeHandler;
  onSelect?: (index: number) => void;
  selected?: number;
  value: ImageValue[];
  containerProps?: BoxProps;
  itemProps?: BoxProps;
  cropping?: boolean;
  onDelete?: (index: number) => void | undefined | null;
  isDraggingEnabled?: boolean;
  showPreview?: boolean;
};

const Images: FC<ImagesProps> = ({
  containerProps,
  disabled,
  itemProps,
  max,
  onChange = () => {
    // eslint-disable-next-line no-console
    console.log('onChange');
  },
  onSelect,
  selected = 0,
  value,
  cropping,
  onDelete,
  showCrow = true,
  isDraggingEnabled = true,
  addabled = true,
  showPreview = false,
}) => {
  const { product } = useConfig();
  const [imagePreview, setImagePreview] = useState<string | undefined | null>(undefined);
  const selectHandler = useSelect(onSelect);
  const addHandler = useAdd(value, onChange);
  const changeHandler = useChange(value, onChange);

  const handlePreview = useSelect((index: number) => {
    const image = value[index] as FileReadModel;

    setImagePreview(image.path);
  });

  const handleModalClose = () => {
    setImagePreview(null);
  };

  const maxItems = max ?? product.maxImages;
  const currentSelected = Math.min(Math.max(selected, 0), maxItems);

  const onDragged = (dragIndex: number, hoverIndex: number) => {
    const firstItem = value[dragIndex];
    const secondItem = value[hoverIndex];

    value[hoverIndex] = firstItem;
    value[dragIndex] = secondItem;

    onChange?.([...value]);
  };

  return (
    <Box {...containerProps}>
      {value.map((image, index) => (
        <Box key={typeof image === 'string' ? image : image.id} {...itemProps}>
          <Image
            index={index}
            showCrow={showCrow}
            onClick={showPreview ? handlePreview : selectHandler}
            onSuccess={changeHandler}
            selected={currentSelected === index}
            value={image}
            moveCard={onDragged}
            onDelete={onDelete}
            isDraggingEnabled={isDraggingEnabled}
          >
            {image}
          </Image>
        </Box>
      ))}

      <Modal
        open={!!imagePreview}
        onCancel={handleModalClose}
        cancelText="Закрыть"
        width="90%"
        style={{
          textAlign: 'center',
        }}
        okButtonProps={{ style: { display: 'none' } }}
      >
        <img src={String(imagePreview)} style={{ maxHeight: '70vh' }} alt="" />
      </Modal>

      {value.length < maxItems && addabled && (
        <Box {...itemProps}>
          <UploadImage accept="image/*" cropping={cropping} disabled={disabled} onComplete={addHandler} />
        </Box>
      )}
    </Box>
  );
};

export { Images };
export type { ImagesProps };
