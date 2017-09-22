function OpenURL(element) {
    var url = $(element).attr('data-url');
    window.location.replace('/' + url);
}

//  Executes the associated function to generate
//  content for the current area
function GenerateContent(button) {
    //  Click the sibling that holds a reference to the 
    //  ajax function that requests the content.
    var hiddenButton = $(button).parent('.div').siblings('.btn-hidden');
    var quantity = $(button).siblings('.label-quantity').text();
    hiddenButton.attr('data-quantity', quantity);
    hiddenButton.click();
}

//  Changes the value adjustment button.
//  Alternate between assignment and addition.
function SwapOperation(button) {
    //  swap the display value of this button
    var currentOperation = $(button).val();
    var newOperation = $(button).attr('data-value');
    $(button).val(newOperation);
    $(button).attr('data-value', currentOperation);

    $(button).siblings('.btn-adjust-value').each(function (index, element) {
        var adjustButton = $(element);

        //  swap the display of the adjustment buttons
        adjustButton.val(newOperation + adjustButton.attr('data-value'));
    });
}

//  Changes the displayed value according to the data-value
//  of the button and the current value of the operation button.
function AdjustQuantity(button) {
    //  This will be either "+" or "="
    var operation = $(button).siblings('.btn-operation').val();
    //  This will be a number, e.g., 1, 5, 100
    var quantity = parseInt($(button).attr('data-value'), 10);
    //  This is the current value, e.g., "3", "47", "101"
    var current = $(button).siblings('.label-quantity');

    switch (operation) {
        case "+":
            current.text(parseInt(current.text(), 10) + quantity);
            break;
        case "=":
            current.text(quantity);
            break;
    }
}