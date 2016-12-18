import Button from '../app/components/Button';
import ImagePair from './ImagePair.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { deleteAllImages } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';

@connect(state => ({
  images: state.api.getIn(['images', 'data']),
  imagesPendingDelete: state.api.getIn(['images', 'pendingDelete']),
  sessionId: state.app.get('sessionId'),
}), { deleteAllImages })
export default class Images extends Component {

  static propTypes = {
    deleteAllImages: RPT.func,
    images: RPT.array,
    imagesPendingDelete: RPT.bool,
    sessionId: RPT.string
  }

  renderImage(key, reference) {
    const { imageUrl, contourImageUrl, cannyTreshodLinking, cannyTreshold, timeline } = reference.toJS();
    return (
      <ImagePair
        cannyTreshodLinking={cannyTreshodLinking}
        cannyTreshold={cannyTreshold}
        contourUrl={contourImageUrl}
        id={key}
        imageUrl={imageUrl}
        timeline={timeline}
        pairType="image"
      />
    );
  }

  render() {
    const { images, imagesPendingDelete, deleteAllImages, sessionId } = this.props;

    return (
      <div>
        <div>
          <h1>Obrázky k porovnání</h1>
          <div style={styles.imagesWrapper}>
            {images && images.map((x, key) => this.renderImage(key, x))}
          </div>
          {images && images.size > 0 && <Button backgroundColor="secondary" disabled={imagesPendingDelete} onClick={() => deleteAllImages(sessionId)}>Smazat vše</Button>}
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture', sessionId }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/images"
          >
            <Button>
              Klikněte nebo přetáhněte obrázky k porovnání (JPG, PNG)
            </Button>
          </FileUpload>
        </div>
      </div>
    );
  }
}

const styles = {
  imagesWrapper: {
    overflow: 'auto',
    height: 'auto',
    clear: 'both'
  }
};
