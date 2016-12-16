import ImagePair from './ImagePair.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { deleteAllImages, putImage } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';

@connect(state => ({
  images: state.api.getIn(['images', 'data']),
  sessionId: state.app.get('sessionId'),
}), { deleteAllImages })
export default class Images extends Component {

  static propTypes = {
    deleteAllImages: RPT.func,
    images: RPT.array,
    sessionId: RPT.string
  }

  renderImage(key, reference) {
    const { imageUrl, contourImageUrl, cannyTreshodLinking, cannyTreshold } = reference.toJS();
    console.log(reference.toJS())
    return (
      <ImagePair
        cannyTreshodLinking={cannyTreshodLinking}
        cannyTreshold={cannyTreshold}
        contourUrl={contourImageUrl}
        id={key}
        imageUrl={imageUrl}
        pairType="image"
      />
    );
  }

  render() {
    const { images, deleteAllImages, sessionId } = this.props;

    return (
      <div>
        <div>
          <h1>Obrázky k porovnání</h1>
          {images && images.map((x, key) => this.renderImage(key, x))}
          <button onClick={() => deleteAllImages()}>Smazat vše</button>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture', sessionId: sessionId }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/images"
          >
            <button>
              Klikněte nebo přetáhněte obrázky k porovnání (JPG)
            </button>
          </FileUpload>
        </div>
      </div>
    );
  }
}
